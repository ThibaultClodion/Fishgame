using UnityEngine;
using UnityEngine.InputSystem;

#if UNITY_EDITOR
using UnityEditor;
#endif

// The point of this processor is to make the mouse behave like a gamepad
// aka it will have 0,0 at the center of the screen and go to -1,-1 & 1,1

#if UNITY_EDITOR
[InitializeOnLoad]
#endif
public class MouseGamepadEmulatorProcessor : InputProcessor<Vector2>
{
	#if UNITY_EDITOR
	static MouseGamepadEmulatorProcessor()
	{
		Initialize();
	}
	#endif

	[RuntimeInitializeOnLoadMethod]
	static void Initialize()
	{
		InputSystem.RegisterProcessor<MouseGamepadEmulatorProcessor>();
	}

	public override Vector2 Process(Vector2 value, InputControl control)
	{
		value.x = ((value.x*2.0f)/Screen.width) - 1.0f;
		value.y = ((value.y*2.0f)/Screen.height) - 1.0f;
		return value;
	}
}
