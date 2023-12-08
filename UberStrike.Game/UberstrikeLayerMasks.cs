public static class UberstrikeLayerMasks {
	public static readonly int IdentificationMask = LayerUtil.CreateLayerMask(UberstrikeLayer.RemotePlayer, UberstrikeLayer.Default, UberstrikeLayer.GloballyLit);
	public static readonly int CrouchMask = ~LayerUtil.CreateLayerMask(UberstrikeLayer.LocalPlayer, UberstrikeLayer.Controller, UberstrikeLayer.IgnoreRaycast, UberstrikeLayer.Trigger, UberstrikeLayer.TransparentFX);
	public static readonly int ShootMask = ~LayerUtil.CreateLayerMask(UberstrikeLayer.LocalPlayer, UberstrikeLayer.Controller, UberstrikeLayer.IgnoreRaycast, UberstrikeLayer.Trigger, UberstrikeLayer.TransparentFX, UberstrikeLayer.Raidbot);
	public static readonly int ShootMaskRemotePlayer = ~LayerUtil.CreateLayerMask(UberstrikeLayer.Controller, UberstrikeLayer.IgnoreRaycast, UberstrikeLayer.Trigger, UberstrikeLayer.TransparentFX, UberstrikeLayer.Raidbot);
	public static readonly int RemoteRocketMask = ~LayerUtil.CreateLayerMask(UberstrikeLayer.RemotePlayer, UberstrikeLayer.Controller, UberstrikeLayer.Teleporter, UberstrikeLayer.LocalProjectile, UberstrikeLayer.RemoteProjectile, UberstrikeLayer.IgnoreRaycast, UberstrikeLayer.Trigger, UberstrikeLayer.TransparentFX);
	public static readonly int LocalRocketMask = ~LayerUtil.CreateLayerMask(UberstrikeLayer.LocalPlayer, UberstrikeLayer.Controller, UberstrikeLayer.Teleporter, UberstrikeLayer.LocalProjectile, UberstrikeLayer.RemoteProjectile, UberstrikeLayer.IgnoreRaycast, UberstrikeLayer.Trigger, UberstrikeLayer.TransparentFX, UberstrikeLayer.Raidbot);
	public static readonly int ExplosionMask = LayerUtil.CreateLayerMask(UberstrikeLayer.LocalPlayer, UberstrikeLayer.RemotePlayer, UberstrikeLayer.Props, UberstrikeLayer.Raidbot, UberstrikeLayer.Ragdoll);
	public static readonly int GrenadeCollisionMask = LayerUtil.CreateLayerMask(UberstrikeLayer.RemotePlayer);
	public static readonly int GrenadeMask = LayerUtil.CreateLayerMask(UberstrikeLayer.LocalPlayer, UberstrikeLayer.RemotePlayer, UberstrikeLayer.Props, UberstrikeLayer.Raidbot);
	public static readonly int ProtectionMask = ~LayerUtil.CreateLayerMask(UberstrikeLayer.LocalProjectile, UberstrikeLayer.RemoteProjectile, UberstrikeLayer.LocalPlayer, UberstrikeLayer.RemotePlayer, UberstrikeLayer.Controller, UberstrikeLayer.Props, UberstrikeLayer.Ragdoll, UberstrikeLayer.IgnoreRaycast, UberstrikeLayer.Trigger, UberstrikeLayer.TransparentFX);
	public static readonly int WaterMask = LayerUtil.CreateLayerMask(UberstrikeLayer.Water);
}
