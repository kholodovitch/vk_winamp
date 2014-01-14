using System;
using System.Collections.Generic;
using System.Text;
using ApiCore.Utils.Authorization.Exceptions;

namespace ApiCore.Utils.Authorization
{
    public class OAuth: IOAuthProvider
    {
	    private readonly string _email;
	    private readonly string _pass;

	    public OAuth(string email, string pass)
	    {
		    _email = email;
		    _pass = pass;
	    }

	    public SessionInfo Authorize(int appId, string scope, string display)
        {
			if (!string.IsNullOrEmpty(_email) && !string.IsNullOrEmpty(_pass))
			{
				var authHidden = new OAuthHidden(_email, _pass);
				authHidden.Authorize(appId, scope, display);
				return authHidden.SessionData;
			}

		    OAuthWnd wnd = new OAuthWnd(appId, scope, display);
            wnd.ShowDialog();
            if (wnd.Authenticated)
                return wnd.SessionData;
            else
                throw new AuthorizationFailedException("Authorization failed!");
        }
    }
}
