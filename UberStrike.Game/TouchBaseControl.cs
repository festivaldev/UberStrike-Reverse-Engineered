using UnityEngine;

public abstract class TouchBaseControl {
	public virtual bool Enabled { get; set; }
	public virtual Rect Boundary { get; set; }

	public TouchBaseControl() {
		Singleton<TouchController>.Instance.AddControl(this);
	}

	public virtual void FirstUpdate() { }
	public virtual void UpdateTouches(Touch touch) { }
	public virtual void FinalUpdate() { }
	public virtual void Draw() { }

	~TouchBaseControl() {
		Singleton<TouchController>.Instance.RemoveControl(this);
	}
}
