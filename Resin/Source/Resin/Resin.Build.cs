// Copyright Epic Games, Inc. All Rights Reserved.

using UnrealBuildTool;

public class Resin : ModuleRules
{
	public Resin(ReadOnlyTargetRules Target) : base(Target)
	{
		PCHUsage = PCHUsageMode.UseExplicitOrSharedPCHs;

		PublicDependencyModuleNames.AddRange(new string[] { "Core", "CoreUObject", "Engine", "InputCore", "HeadMountedDisplay" });
	}
}
