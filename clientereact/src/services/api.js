import axios from "axios";

const api = axios.create({
    baseURL : "https://localhost:7006",
})

export default api;