using UberStrike.Core.Models;

public class SoundSimulator {
	private CharacterConfig character;
	private bool isGrounded = true;

	public SoundSimulator(CharacterConfig character) {
		this.character = character;
	}

	public void Update() {
		if (character == null || character.Avatar == null || character.State == null) {
			return;
		}

		var flag = (byte)(character.State.MovementState & (MoveStates.Grounded | MoveStates.Wading | MoveStates.Swimming | MoveStates.Diving)) != 0;
		var flag2 = (character.State.Is(MoveStates.Diving) && character.State.KeyState != KeyState.Still) || (byte)(character.State.KeyState & KeyState.Walking) != 0;

		if (!isGrounded && character.State.Is(MoveStates.Grounded)) {
			character.Avatar.Decorator.PlayFootSound(character.WalkingSoundSpeed);
		} else if (flag && flag2) {
			if (character.State.Is(MoveStates.Wading)) {
				character.Avatar.Decorator.PlayFootSound(character.WalkingSoundSpeed, FootStepSoundType.Water);
			} else if (character.State.Is(MoveStates.Swimming)) {
				character.Avatar.Decorator.PlayFootSound(character.SwimSoundSpeed, FootStepSoundType.Swim);
			} else if (character.State.Is(MoveStates.Diving)) {
				character.Avatar.Decorator.PlayFootSound(character.DiveSoundSpeed, FootStepSoundType.Dive);
			} else {
				character.Avatar.Decorator.PlayFootSound(character.WalkingSoundSpeed);
			}
		}

		isGrounded = character.State.Is(MoveStates.Grounded);
	}
}
