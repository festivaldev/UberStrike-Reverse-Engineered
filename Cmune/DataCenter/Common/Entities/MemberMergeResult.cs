namespace Cmune.DataCenter.Common.Entities {
	public enum MemberMergeResult {
		Ok,
		CmidNotFound,
		CmidAlreadyLinkedToEsns = 3,
		EsnsAlreadyLinkedToCmid,
		InvalidData
	}
}
