namespace BookingAPI.Data; 

public static class Routes {
    public static string DatabaseApiUrl => "https://localhost:7100";

    public static class UserEndpoints {
        public static string UserApiUrl => $"{DatabaseApiUrl}/user";
        
        public static string Add => $"{UserApiUrl}/add";
        // public static string GetByUsername(string username) => $"{UserApiUrl}?username={username}";
        // public static string GetById(int id) => $"{UserApiUrl}/{id}";
    }
}