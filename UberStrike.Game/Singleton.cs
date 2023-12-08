using System;
using System.Reflection;

public class Singleton<T> : IDisposable where T : Singleton<T> {
	private static volatile T _instance;
	private static object _lock = new object();

	public static T Instance {
		get {
			if (_instance == null) {
				var @lock = _lock;

				lock (@lock) {
					if (_instance == null) {
						var constructor = typeof(T).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[0], null);

						if (constructor == null || constructor.IsAssembly) {
							throw new Exception(string.Format("A private or protected constructor is missing for '{0}'.", typeof(T).Name));
						}

						_instance = (T)constructor.Invoke(null);
					}
				}
			}

			return _instance;
		}
	}

	public void Dispose() {
		OnDispose();
		_instance = (T)null;
	}

	protected virtual void OnDispose() { }
}
