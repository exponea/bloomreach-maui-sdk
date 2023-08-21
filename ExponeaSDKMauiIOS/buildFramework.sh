#!/bin/bash

# Builds a fat library for a given xcode project (framework)

echo "Define parameters"
IOS_SDK_VERSION="16.4" # xcodebuild -showsdks
SWIFT_PROJECT_NAME="ExponeaSDKMauiIOS"
declare -a frameworks=("ExponeaSDKProxy" "SwiftSoup" "ExponeaSDK" "ExponeaSDKShared" "ExponeaSDKObjC" "AnyCodable")

SWIFT_BUILD_DIR="build"
SWIFT_BUILD_RELATIVE_PATH="./$SWIFT_BUILD_DIR"
SWIFT_BUILD_ABSOLUTE_PATH="$(pwd)/$SWIFT_BUILD_DIR"
SWIFT_OUTPUT_PATH="./frameworks"
SWIFT_OUTPUT_HEADER_FILE="$SWIFT_OUTPUT_PATH/$SWIFT_PROJECT_NAME.framework/Headers/$SWIFT_PROJECT_NAME-Swift.h"

echo "Build iOS framework for simulator and device"
rm -Rf "$SWIFT_BUILD_RELATIVE_PATH"
xcodebuild -sdk iphonesimulator -workspace "$SWIFT_PROJECT_NAME.xcworkspace" -scheme "$SWIFT_PROJECT_NAME" -configuration Release SKIP_INSTALL=NO BUILD_LIBRARIES_FOR_DISTRIBUTION=YES CONFIGURATION_BUILD_DIR="$SWIFT_BUILD_ABSOLUTE_PATH/Release-iphonesimulator"
xcodebuild -sdk iphonesimulator -workspace "$SWIFT_PROJECT_NAME.xcworkspace" -scheme "$SWIFT_PROJECT_NAME" -configuration Debug SKIP_INSTALL=NO BUILD_LIBRARIES_FOR_DISTRIBUTION=YES CONFIGURATION_BUILD_DIR="$SWIFT_BUILD_ABSOLUTE_PATH/Debug-iphonesimulator"
xcodebuild -sdk iphoneos -workspace "$SWIFT_PROJECT_NAME.xcworkspace" -scheme "$SWIFT_PROJECT_NAME" -configuration Release SKIP_INSTALL=NO BUILD_LIBRARIES_FOR_DISTRIBUTION=YES CONFIGURATION_BUILD_DIR="$SWIFT_BUILD_ABSOLUTE_PATH/Release-iphoneos"
xcodebuild -sdk iphoneos -workspace "$SWIFT_PROJECT_NAME.xcworkspace" -scheme "$SWIFT_PROJECT_NAME" -configuration Debug SKIP_INSTALL=NO BUILD_LIBRARIES_FOR_DISTRIBUTION=YES CONFIGURATION_BUILD_DIR="$SWIFT_BUILD_ABSOLUTE_PATH/Debug-iphoneos"

echo "Create fat binaries for Release-iphoneos and Release-iphonesimulator configuration"
echo "Copy one build as a fat framework"
cp -R "$SWIFT_BUILD_RELATIVE_PATH/Release-iphoneos" "$SWIFT_BUILD_RELATIVE_PATH/Release-fat"

echo "Create fat binaries for Debug-iphoneos and Debug-iphonesimulator configuration"
echo "Copy one build as a fat framework"
cp -R "$SWIFT_BUILD_RELATIVE_PATH/Debug-iphoneos" "$SWIFT_BUILD_RELATIVE_PATH/Debug-fat"

rm -Rf "$SWIFT_OUTPUT_PATH"
mkdir "$SWIFT_OUTPUT_PATH"

combine_to_fat_debug () {

	echo "Combine modules from another build with the fat framework modules for $1"
	cp -R "$SWIFT_BUILD_RELATIVE_PATH/Debug-iphonesimulator/$1.framework/Modules/$1.swiftmodule/" "$SWIFT_BUILD_RELATIVE_PATH/Debug-fat/$1.framework/Modules/$1.swiftmodule/"

	lipo -remove arm64 "$SWIFT_BUILD_RELATIVE_PATH/Debug-iphonesimulator/$1.framework/$1" -output "$SWIFT_BUILD_RELATIVE_PATH/Debug-iphonesimulator/$1.framework/$1"

	echo "Combine iphoneos + iphonesimulator configuration as fat libraries"
	lipo -create -output "$SWIFT_BUILD_RELATIVE_PATH/Debug-fat/$1.framework/$1" "$SWIFT_BUILD_RELATIVE_PATH/Debug-iphoneos/$1.framework/$1" "$SWIFT_BUILD_RELATIVE_PATH/Debug-iphonesimulator/$1.framework/$1"

	echo "Verify results"
	lipo -info "$SWIFT_BUILD_RELATIVE_PATH/Debug-fat/$1.framework/$1"

	echo "Copy fat frameworks to the output folder"
	cp -Rf "$SWIFT_BUILD_RELATIVE_PATH/Debug-fat/$1.framework" "$SWIFT_OUTPUT_PATH"
}

combine_to_fat_release () {

	echo "Combine modules from another build with the fat framework modules for $1"
	cp -R "$SWIFT_BUILD_RELATIVE_PATH/Release-iphonesimulator/$1.framework/Modules/$1.swiftmodule/" "$SWIFT_BUILD_RELATIVE_PATH/Release-fat/$1.framework/Modules/$1.swiftmodule/"

	lipo -remove arm64 "$SWIFT_BUILD_RELATIVE_PATH/Release-iphonesimulator/$1.framework/$1" -output "$SWIFT_BUILD_RELATIVE_PATH/Release-iphonesimulator/$1.framework/$1"

	echo "Combine iphoneos + iphonesimulator configuration as fat libraries"
	lipo -create -output "$SWIFT_BUILD_RELATIVE_PATH/Release-fat/$1.framework/$1" "$SWIFT_BUILD_RELATIVE_PATH/Release-iphoneos/$1.framework/$1" "$SWIFT_BUILD_RELATIVE_PATH/Release-iphonesimulator/$1.framework/$1"

	echo "Verify results"
	lipo -info "$SWIFT_BUILD_RELATIVE_PATH/Release-fat/$1.framework/$1"

	echo "Copy fat frameworks to the output folder"
	cp -Rf "$SWIFT_BUILD_RELATIVE_PATH/Release-fat/$1.framework" "$SWIFT_OUTPUT_PATH"
}

for i in `find $SWIFT_BUILD_RELATIVE_PATH -name "*.framework" -type d -mindepth 1 -maxdepth 2 | xargs basename ".framework" | sort | uniq`; do
  if [ $i = ".framework" ]
  then
    continue
  fi
  combine_to_fat_release "$(basename $i .framework)"
  combine_to_fat_debug "$(basename $i .framework)"
done

# Some invalid definitions are produced, i.e. IAuthorizationProviderType so we need to update ApiDefinition manually
# uncomment only occasionally
#echo "Generating binding api definition and structs"
#sharpie bind --sdk=iphoneos --output="./MauiApiDef" --namespace="ExponeaSdk" --scope="$SWIFT_OUTPUT_PATH/$SWIFT_PROJECT_NAME.framework/Headers/" "$SWIFT_OUTPUT_PATH/$SWIFT_PROJECT_NAME.framework/Headers/$SWIFT_PROJECT_NAME-Swift.h"

echo "Done!"