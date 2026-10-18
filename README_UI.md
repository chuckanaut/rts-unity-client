# Additional Unity README notes

This commit added UI panels and networking scripts for:
- Avatar upload (placeholder flow)
- Lobby creation/search/join
- Chat panel using backend endpoints
- Shop purchase placeholder (integrate Play Billing)

To test the flows in editor:
- Set ApiClient.BaseUrl to your backend (http://localhost:3000)
- Create simple UI using the provided scripts and wire references in the inspector
- Use POST /api/auth/google to create a test user, copy the returned user.id into `userId` fields on UI controllers
