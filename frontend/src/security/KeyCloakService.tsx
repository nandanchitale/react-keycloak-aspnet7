import Keycloak from "keycloak-js";

const keycloakInstance = new Keycloak();

/**
 * Initializes Keycloak instance and calls the provided callback function if successfully authenticated.
 *
 * @param onAuthenticatedCallback
 */
const Login = (onAuthenticatedCallback: Function) => {
    keycloakInstance
        .init({ onLoad: "login-required" })
        .then(function (authenticated) {
            authenticated ? onAuthenticatedCallback() : alert("non authenticated");
        })
        .catch((e) => {
            console.dir(e);
            console.log(`keycloak init exception: ${e}`);
        });
};

const Username = () => keycloakInstance.tokenParsed?.preferred_username;

const userRoles = () => {
    if (keycloakInstance.resourceAccess === undefined || keycloakInstance.resourceAccess["demo_backend"] === undefined) return undefined;
    else return keycloakInstance.resourceAccess["demo_backend"].roles;
};

const Logout = keycloakInstance.logout;

const isLoggedIn = () => !keycloakInstance.token;

const getToken = () => keycloakInstance.token;

const doLogin = keycloakInstance.login;

const updateToken = (successCallback: any) => keycloakInstance.updateToken(5).then(successCallback).catch(doLogin);

const KeyCloakService = {
    CallLogin: Login,
    GetUserName: Username,
    GetUserRoles: userRoles,
    CallLogout: Logout,
    IsLoggedIn: isLoggedIn,
    GetToken: getToken,
    updateToken: updateToken
};

export default KeyCloakService;