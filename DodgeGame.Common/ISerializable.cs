using Riptide;

namespace DodgeGame.Common
{
    public interface ISerializable<out T>
    {
        void Serialize(Message message);

        static T Deserialize(Message message)
        {
            return default;
        }
    }
}