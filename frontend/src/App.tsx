import React from 'react';
import logo from './logo.svg';
import './App.css';
import KeyCloakService from './security/KeyCloakService';
import axios from 'axios';

function logout() {
  KeyCloakService.CallLogout();
}

function getData() {
  console.log("KC_TOKEN: " + KeyCloakService.GetToken());

  const config = {
    headers: {
      authorization: `Bearer ${KeyCloakService.GetToken()}`,
    },
  };

  axios.get("https://localhost:7158/WeatherForecast", config)
    .then((res) => alert(JSON.stringify(res.data)))
    .catch((res) => console.error(res));
}

function App() {
  return (
    <div className="App">
      <header className="App-header">
        <img src={logo} className="App-logo" alt="logo" />
        <p>
          Edit <code>src/App.tsx</code> and save to reload.
        </p>
        <p>Welcome {KeyCloakService.GetUserName()}</p>
        <p>User Roles : {KeyCloakService.GetUserRoles()?.join(" ") || 'No roles available'} </p>
        <p>Keycloak Token : <span style={{ "width": "1000px", "display": "block", "overflowWrap": "break-word" }}>
          {KeyCloakService.GetToken()}
        </span></p>
        <button onClick={logout}>Log Out</button>
        <button onClick={getData}>WeatherForecast</button>
        <a
          className="App-link"
          href="https://reactjs.org"
          target="_blank"
          rel="noopener noreferrer"
        >
          Learn React
        </a>
      </header>
    </div >
  );
}

export default App;
