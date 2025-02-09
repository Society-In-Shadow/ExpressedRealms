To change URL's

- api app container
    - Update CORS to point to new origin
    - In Secrets, update front-end-base-url to new front end URL
    - In Secrets, update client-cookie-domain to new base domain front end url
    - Restart the app
- You need to register new A, CNAME and certificates for the domain
- You need to update .env.production file with the new endpoints
- You need to update nginx.conf with the new API endpoint
- Email appears to work regardless of domain
- See this for more info [PR for Updating URL](https://github.com/Society-In-Shadow/ExpressedRealms/pull/194)