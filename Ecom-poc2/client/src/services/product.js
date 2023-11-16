export const product=
    function getAll() {
        return axiosInstance.request({
            method: 'GET',
            url: `/item`
        })
    }
    function get(id){
        return axiosInstance.request({
            method:"GET",
            url:"/user"+id
        })
    }
  ;
