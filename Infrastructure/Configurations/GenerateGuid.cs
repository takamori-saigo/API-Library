namespace Infrastructure.Configurations;

public class GenerateGuid
{
    public static Guid GetGuid(DateTime dateTime, int sequence)
    {
        var bytes = new byte[16];
        BitConverter.GetBytes(dateTime.Ticks + sequence).CopyTo(bytes, 0);
        return new Guid(bytes);
    }
}