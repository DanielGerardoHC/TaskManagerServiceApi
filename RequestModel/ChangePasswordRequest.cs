namespace TaskManagerServiceApi.RequestModel;

public class ChangePasswordRequest
{
    public int? UserId { get; set; }
    public string? UserName { get; set; }
    public string? OldPassword { get; set; }
    public string? NewPassword { get; set; }
}