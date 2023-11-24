import {axiosInstance} from "../utils/axiosInstance";
export const verifyPhoneServices = 
function sendOtp(num){
    return axiosInstance.request({
        method:"GET",
        url:"/user"
    })
}