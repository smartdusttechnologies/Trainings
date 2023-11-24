import axios from "axios"
import { BASE_URL }  from "./app_uri";
export const axiosInstance = axios.create({
    baseURL: BASE_URL,
    headers:{
        "content-type": "application/json"
    }
});