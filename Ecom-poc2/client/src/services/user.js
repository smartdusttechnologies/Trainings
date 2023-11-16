// import axiosInstance which has been created
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
     function get(id){
        // return axiosInstance.request({
        //     method:"GET",
        //     url:"/"
        // })
     /// Method will be get 
     /// and change url to (`post/${id}`)
    }
  ;
