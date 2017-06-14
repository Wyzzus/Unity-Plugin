//
//  MotionDnaApplication.h
//  MotionDnaApplication
//
//  Created by Navisens, Inc on 8/19/15.
//  Copyright (c) 2015 Navisens, Inc. All rights reserved.
//

#import <UIKit/UIKit.h>
#import <CoreMotion/CoreMotion.h>
//! Project version number for MotionDnaApplication.
FOUNDATION_EXPORT double MotionDnaApplicationVersionNumber;

//! Project version string for MotionDnaApplication.
FOUNDATION_EXPORT const unsigned char MotionDnaApplicationVersionString[];

// In this header, you should import all the public headers of your framework using statements like #import <MotionDnaApplication/PublicHeader.h>

#import "MotionDna.h"

@interface MotionDnaSDK: NSObject

-(void)runMotionDna:(NSString*)ID receiver:(id)receiver;
-(void)runMotionDna:(NSString*)ID;
-(void)receiveMotionDna:(MotionDna*)motionDna;

-(void)runMotionDnaWithoutMotionManager:(NSString*)ID;
-(void)failureToAuthenticate:(NSString*)msg;
-(void)reportSensorTiming:(double)dt Msg:(NSString*)msg;
-(void)receiveDeviceMotion:(CMDeviceMotion*)deviceMotion;
-(void)setFloorNumber:(int)floor;
-(void)setLocationLatitude:(double)latitude Longitude:(double)longitude AndHeadingInDegrees:(double)heading;
-(void)setLocationAndHeadingGPSMag;
-(void)setLocationLatitude:(double)latitude Longitude:(double)longitude;
-(void)setLocationGPSOnly;
-(void)setHeadingMagInDegrees;
-(void)setHeadingInDegrees:(double)heading;
-(void)setLocationBeacon;
-(void)setLocationNavisens;

-(void)pause;
-(void)resume;
-(void)startCalibration;
-(void)stop;
+(NSString*)checkSDKVersion;
-(NSString*)getDeviceID;
-(NSString*)getDeviceName;

-(void)setMapCorrectionEnabled:(BOOL)state;
-(void)setCallbackUpdateRateInMs:(double)rate;
-(void)setNetworkUpdateRateInMs:(double)rate;
-(void)setBinaryFileLoggingEnabled:(BOOL)state;
-(void)setExternalPositioningState:(ExternalPositioningState)state;
-(void)setGreetingEnabled:(BOOL)state;
-(void)startUDPHost:(NSString*)host AndPort:(NSString*)port;
-(void)startUDP;
-(void)stopUDP;
-(void)setBackgroundModeEnabled:(BOOL)state;
-(void)logBeaconWithName:(NSString*)name ID:(NSString*)id RSSI:(int)RSSI andTimestamp:(double)timestamp;
-(void)setBeaconRangingEnabled:(BOOL)state;
-(void)setBeaconCorrectionsEnabled:(BOOL)state;
-(void)removeBeaconRegionWithUUID:(NSUUID *)proximityUUID andIdentifier:(NSString*)identifierForVendor;
-(void)registerBeaconRegionWithUUID:(NSUUID *)proximityUUID andIdentifier:(NSString*)identifier;
-(BOOL)isRangingBLE;
-(void)setPowerMode:(PowerConsumptionMode)mode;
-(void)setVehicleModeEnabled:(BOOL)state;
-(void)setTransportAutoswitchModeEnabled:(BOOL)state;
-(void)setLocalHeadingOffsetInDegrees:(double)hdg;
-(void)setCartesianOffsetInMetersX:(double)x Y:(double)y;
-(void)setARModeEnabled:(BOOL)state;
-(void)setEstimationMode:(EstimationMode)mode;
-(void)resetLocalEstimation;

@end
