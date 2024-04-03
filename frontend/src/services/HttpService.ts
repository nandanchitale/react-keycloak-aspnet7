
import axios from "axios";
import KeyCloakService from "../security/KeyCloakService";

const HttpMethods = {
    GET: "GET",
    POST: "POST",
    DELETE: "DELETE",
};

const options = {
    baseURL: "https://localhost:7158/WeatherForecast",
    timeout: 300000,
    headers: {
        'Content-Type': 'application/json',
    }
}

const _axios = axios.create();

const configure = () => {
    _axios.interceptors.request.use((config: any) => {
        if (KeyCloakService.IsLoggedIn()) {
            const cb = () => {
                config.headers.Authorization = `Bearer ${KeyCloakService.GetToken()}`;
                return Promise.resolve(config);
            };
            const updatedConfig = KeyCloakService.updateToken(cb);
            return updatedConfig ?? config; // Ensure a valid config is returned
        }
        return config;
    });
};

export function createAxiosClient() {
    const client = axios.create(options);

    client.interceptors.request.use(
        (config) => {
            if (KeyCloakService.IsLoggedIn()) {
                const token = KeyCloakService.GetToken();
                if (token) {
                    config.headers.Authorization = "Bearer " + token;
                }
            }
            return config;
        },
        (error) => {
            return Promise.reject(error);
        }
    );

    return client;
}

const getAxiosClient = () => createAxiosClient();

const HttpService = {
    HttpMethods,
    configure,
    getAxiosClient,
};

export default HttpService;