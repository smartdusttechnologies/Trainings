export const user=
    function getAll() {
        return axiosInstance.request({
            method: 'GET',
            url: `/users`
        })
    }
    function save(data){
        return axiosInstance.request({
            method:"POST",
            url:"/user",
            data:data
        })
    }
   
  ;
