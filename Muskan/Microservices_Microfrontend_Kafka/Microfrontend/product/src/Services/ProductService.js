const API_SERVER = "http://localhost:6004/catalog-service";

export const getProduct = () => {
  return fetch(`${API_SERVER}/products`)
    .then((res) => res.json())
    .then((data) => data.products);
};

export const getProductById = (id) => {
  return fetch(`${API_SERVER}/products/${id}`).then((res) => res.json());
};
