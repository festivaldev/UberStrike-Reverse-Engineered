using System;
using Cmune.DataCenter.Common.Entities;
using UnityEngine;

public class ClientConfiguration {
	public string WebServiceBaseUrl { get; set; }
	public string ItemAssetBundlePath { get; set; }
	public string MapAssetBundlePath { get; set; }
	public string ImagePath { get; set; }
	public string ContentRouterBaseUrl { get; set; }
	public string FacebookAppId { get; set; }
	public string CmuneBootstrapUrl { get; set; }
	public ChannelType ChannelType { get; set; }
	public string PaymentBundleUrl { get; set; }
	public bool IsDebug { get; set; }

	public ClientConfiguration() {
		WebServiceBaseUrl = string.Empty;
		ItemAssetBundlePath = string.Empty;
		MapAssetBundlePath = string.Empty;
		ImagePath = string.Empty;
		ContentRouterBaseUrl = string.Empty;
		FacebookAppId = string.Empty;
		ChannelType = ChannelType.WebPortal;
		PaymentBundleUrl = string.Empty;
		CmuneBootstrapUrl = string.Empty;
		IsDebug = true;
	}

	public void SetChannelType(string value) {
		try {
			ChannelType = (ChannelType)((int)Enum.Parse(typeof(ChannelType), value));
		} catch {
			Debug.LogError("Unsupported ChannelType!");
		}
	}
}
