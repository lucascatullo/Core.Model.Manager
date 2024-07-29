using System.Text.Json;

namespace Core.Models.Manager.Model;

public class EventMessage<TMsg> where TMsg : class
{
    public FullEvent<TMsg> FullEvent { get; set; }
    public string Serialize()
    {
        return JsonSerializer.Serialize(FullEvent);
    }

    public FullEvent<TMsg> TryTobject(string message)
    {
        try
        {
            FullEvent = JsonSerializer.Deserialize<FullEvent<TMsg>>(message)!;
        }
        catch (System.Exception e)
        {
            FullEvent ??= new FullEvent<TMsg>();
            FullEvent.Message = e.Message;
        }
        return FullEvent;
    }


}