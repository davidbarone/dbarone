import { toast } from "../widgets/myToast";

let urlBase = DOTENV.API_DOMAIN;
alert(urlBase);

function handleErrors(response, successMessage) {
  if (!response.ok) {
    throw Error(response.statusText);
  }
  toastSuccess(successMessage);
  return response;
}

function toastSuccess(message) {
  toast.show(message, {
    timeout: 3000,
    position: "bottom-right",
    variant: "success",
  });
}

function toastFailure(message) {
  toast.show(message, {
    timeout: 3000,
    position: "bottom-right",
    variant: "danger",
  });
}

function getPosts() {
  let url = `http://localhost:4000/posts`;
  return fetch(url, {
    mode: "cors",
    headers: {
      "Content-Type": "application/json",
    },
  })
    .then((response) => handleErrors(response, "Posts retrieved successfully."))
    .then((response) => response.json())
    .then((data) => {
      console.log(data.data);
      return data.data;
    })
    .catch((error) => toastFailure(error));
}

export { getPosts };
