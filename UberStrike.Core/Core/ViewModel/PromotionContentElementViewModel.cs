using Cmune.DataCenter.Common.Entities;
using UberStrike.Core.Types;

namespace UberStrike.Core.ViewModel {
	public class PromotionContentElementViewModel {
		public int PromotionContentElementId { get; set; }
		public ChannelType ChannelType { get; set; }
		public ChannelElement ChannelElement { get; set; }
		public string Filename { get; set; }
		public string FilenameTitle { get; set; }
		public int PromotionContentId { get; set; }
		public string AnchorLink { get; set; }
	}
}
