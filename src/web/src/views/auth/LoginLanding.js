import React, { useEffect } from "react";
import { useDispatch } from "react-redux";
import { useHistory } from "react-router";
import { AuthService } from "../../services/authService";
import { setUser } from "../../store/AuthSlice";

export const LoginLanding = () => {
  const dispatch = useDispatch();
  const history = useHistory();

  useEffect(() => {
    AuthService.signinRedirectCallback().then(({ desiredDestination, user }) => {
      const serializedUser = JSON.parse(JSON.stringify(user))
      dispatch(setUser(serializedUser));
      history.push(desiredDestination);
    })
  });
  return <div>Authentication Successful, redirecting...</div>;
};
