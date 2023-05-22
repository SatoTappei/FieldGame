/// <summary>
/// InputSystemで使用する操作を登録する処理を実装するインターフェース
/// Playerの各Behaviorが実装する
/// </summary>
public interface IInputActionRegistrable
{
    public void RegisterInputAction(InputActionRegister register);
}
