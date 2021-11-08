import { AuthService } from "../services/authService";
import { NavLink } from "react-router-dom";
import { useStoreSelector } from "../store";
import logo from "../tempLogo.png";

const NavBar = () => {
  const isAdmin = useStoreSelector((state) => state.auth.isAdmin);
  const isLoggedIn = useStoreSelector((state) => state.auth.isLoggedIn);

  const logoutHandler = () => {
    AuthService.logout();
  };
  const loginHandler = () => {
    AuthService.signinRedirect();
  };
  return (
    <nav className="navbar navbar-expand-lg navbar-dark bg-secondary">
      <div className="container-fluid">
        <NavLink className="navbar-brand" to='/'>
          <img
            src={logo}
            className="img-fluid img-thumbnail rounded-circle m-2 p-2"
            alt="logo"
            width="50"
            height="50"
          />
        </NavLink>
        <button
          className="navbar-toggler"
          type="button"
          data-bs-toggle="collapse"
          data-bs-target="#navbarSupportedContent"
        >
          <span className="navbar-toggler-icon"></span>
        </button>
        <div className="collapse navbar-collapse" id="navbarSupportedContent">
          <ul className="navbar-nav me-auto mb-2 mb-lg-0 justify-content-end container-fluid">
            {isAdmin && (
              <li className="nav-item p-1">
                <NavLink className='btn btn-primary' to="/admin">Admin Page</NavLink>
              </li>
            )}
            <li className="nav-item mx-2 p-1">
              <NavLink className='btn btn-primary' to="/pagedata">PageData</NavLink>
            </li>
            <li className="nav-item mx-2 p-1">
              <NavLink className='btn btn-primary' to="/person">Register</NavLink>
            </li>
            <li className="nav-item mx-2 p-1">
              {isLoggedIn ? (
                <button className="btn btn-primary" onClick={logoutHandler}>
                  Logout
                </button>
              ) : (
                <button className="btn btn-primary" onClick={loginHandler}>
                  Login
                </button>
              )}
            </li>
          </ul>
        </div>
      </div>
    </nav>
  );
};

export { NavBar };
