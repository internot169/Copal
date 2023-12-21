// Copyright Epic Games, Inc. All Rights Reserved.

#include "ResinGameMode.h"
#include "ResinHUD.h"
#include "ResinCharacter.h"
#include "UObject/ConstructorHelpers.h"

AResinGameMode::AResinGameMode()
	: Super()
{
	// set default pawn class to our Blueprinted character
	static ConstructorHelpers::FClassFinder<APawn> PlayerPawnClassFinder(TEXT("/Game/FirstPersonCPP/Blueprints/FirstPersonCharacter"));
	DefaultPawnClass = PlayerPawnClassFinder.Class;

	// use our custom HUD class
	HUDClass = AResinHUD::StaticClass();
}
