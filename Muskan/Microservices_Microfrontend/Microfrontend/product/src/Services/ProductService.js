const API_SERVER = "https://localhost:6064/catalog-service";

export const getProduct = () => {
  return fetch(`${API_SERVER}/products`)
    .then((res) => res.json())
    .then((data) => data.products);
};

export const getProductById = (id) => {
  return fetch(`${API_SERVER}/products/${id}`).then((res) => res.json());
};
