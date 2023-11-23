import datax from './data.json';
import resdata from './restData.json';

export function getRestaurent(id, slug) {
  const   refetch = 7; const networkStatus = 7; const loading = false; const error = undefined; 
  const data = datax;
  return { data, refetch, networkStatus, loading, error };
}
export function getRestaurents(){
return resdata; 
}