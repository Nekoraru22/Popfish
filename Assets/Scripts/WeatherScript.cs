using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;

public class WeatherManager : MonoBehaviour {
    [SerializeField] private string apiKey;
    private const string BaseUrl = "https://opendata.aemet.es/opendata/api";

    [System.Serializable]
    private class ApiResponse {
        public string datos;
    }

    [System.Serializable]
    private class WeatherResponseWrapper {
        public WeatherResponse[] weatherData;
    }

    [System.Serializable]
    private class WeatherResponse {
        public Prediccion prediccion;
    }

    [System.Serializable]
    private class Prediccion {
        public Dia[] dia;
    }

    [System.Serializable]
    private class Dia {
        public Temperatura temperatura;
    }

    [System.Serializable]
    private class Temperatura {
        public float maxima;
        public float minima;
    }

    public IEnumerator GetAverageTemperature(string municipioCode, System.Action<float, string> onComplete) {
        using (var www = UnityWebRequest.Get($"{BaseUrl}/prediccion/especifica/municipio/diaria/{municipioCode}")) {
            www.SetRequestHeader("api_key", apiKey);
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success) {
                var apiResponse = JsonUtility.FromJson<ApiResponse>(www.downloadHandler.text);

                using (var weatherRequest = UnityWebRequest.Get(apiResponse.datos)) {
                    yield return weatherRequest.SendWebRequest();

                    if (weatherRequest.result == UnityWebRequest.Result.Success) {
                        var weatherWrapper = JsonUtility.FromJson<WeatherResponseWrapper>("{\"weatherData\":" + weatherRequest.downloadHandler.text + "}");
                        var dayData = weatherWrapper.weatherData[0].prediccion.dia[0];
                        float averageTemp = (dayData.temperatura.maxima + dayData.temperatura.minima) / 2f;
                        string currentTime = DateTime.Now.ToString("HH:mm:ss");
                        onComplete?.Invoke(averageTemp, currentTime);
                    }
                }
            }
        }
    }

    void Start() {
        StartCoroutine(GetAverageTemperature("03122", (averageTemp, time) => {
            Debug.Log($"Time: {time} - Average temperature: {averageTemp}°C");
        }));
    }
}