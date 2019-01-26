namespace Core.Utilities
{
	/// <summary>
	/// Singleton that persists across multiple scenes
	/// </summary>
	public class SerializedPersistentSingleton<T> : SerializedSingleton<T> where T : SerializedSingleton<T>
	{
		protected override void Awake()
		{
			base.Awake();
			DontDestroyOnLoad(gameObject);
		}
	}
}