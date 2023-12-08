namespace Cmune.DataCenter.Common.Entities {
	public class ConvertEntities {
		public static MemberOperationResult ConvertMemberRegistration(MemberRegistrationResult memberRegistration) {
			var memberOperationResult = MemberOperationResult.Ok;

			switch (memberRegistration) {
				case MemberRegistrationResult.Ok:
					memberOperationResult = MemberOperationResult.Ok;

					break;
				case MemberRegistrationResult.InvalidName:
					memberOperationResult = MemberOperationResult.InvalidName;

					break;
				case MemberRegistrationResult.DuplicateName:
					memberOperationResult = MemberOperationResult.DuplicateName;

					break;
				case MemberRegistrationResult.InvalidHandle:
					memberOperationResult = MemberOperationResult.InvalidHandle;

					break;
				case MemberRegistrationResult.DuplicateHandle:
					memberOperationResult = MemberOperationResult.DuplicateHandle;

					break;
				case MemberRegistrationResult.InvalidEsns:
					memberOperationResult = MemberOperationResult.InvalidEsns;

					break;
			}

			return memberOperationResult;
		}

		public static MemberRegistrationResult ConvertMemberOperation(MemberOperationResult memberOperation) {
			var memberRegistrationResult = MemberRegistrationResult.Ok;

			switch (memberOperation) {
				case MemberOperationResult.Ok:
					memberRegistrationResult = MemberRegistrationResult.Ok;

					break;
				default:
					switch (memberOperation) {
						case MemberOperationResult.InvalidName:
							memberRegistrationResult = MemberRegistrationResult.InvalidName;

							break;
						case MemberOperationResult.OffensiveName:
							memberRegistrationResult = MemberRegistrationResult.OffensiveName;

							break;
					}

					break;
				case MemberOperationResult.DuplicateEmail:
					memberRegistrationResult = MemberRegistrationResult.DuplicateEmail;

					break;
				case MemberOperationResult.DuplicateName:
					memberRegistrationResult = MemberRegistrationResult.DuplicateName;

					break;
				case MemberOperationResult.DuplicateEmailName:
					memberRegistrationResult = MemberRegistrationResult.DuplicateEmailName;

					break;
			}

			return memberRegistrationResult;
		}
	}
}
