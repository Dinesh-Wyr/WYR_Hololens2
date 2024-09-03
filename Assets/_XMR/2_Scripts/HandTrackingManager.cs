using MixedReality.Toolkit;
using MixedReality.Toolkit.Subsystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandTrackingManager : MonoBehaviour
{
    public static HandTrackingManager Instance;

    HandsAggregatorSubsystem handsSubsystem;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public static bool IsLeftTracked()
    {
        bool isFound = Instance.handsSubsystem.TryGetEntireHand(XRNode.LeftHand, out IReadOnlyList<HandJointPose> joints);
        return isFound;
    }
    public static bool IsRightTracked()
    {
        bool isFound = Instance.handsSubsystem.TryGetEntireHand(XRNode.RightHand, out IReadOnlyList<HandJointPose> joints);
        return isFound;
    }

    public static bool IsBothHandsTracked()
    {
        return IsLeftTracked() && IsRightTracked();
    }

    public static Pose GetLeftThumbTip()
    {
        Instance.handsSubsystem.TryGetJoint(TrackedHandJoint.ThumbTip, XRNode.LeftHand, out HandJointPose jointPose);
        return jointPose.Pose;
    }

    public static Pose GetRightThumbTip()
    {
        Instance.handsSubsystem.TryGetJoint(TrackedHandJoint.ThumbTip, XRNode.RightHand, out HandJointPose jointPose);
        return jointPose.Pose;

    }

    public static Pose GetLeftIndexTip()
    {
        Instance.handsSubsystem.TryGetJoint(TrackedHandJoint.IndexTip, XRNode.LeftHand, out HandJointPose jointPose);
        return jointPose.Pose;

    }

    public static Pose GetRightIndexTip()
    {
        Instance.handsSubsystem.TryGetJoint(TrackedHandJoint.IndexTip, XRNode.RightHand, out HandJointPose jointPose);
        return jointPose.Pose;
    }

    public static Pose GetLeftMiddleTip()
    {
        Instance.handsSubsystem.TryGetJoint(TrackedHandJoint.MiddleTip, XRNode.LeftHand, out HandJointPose jointPose);
        return jointPose.Pose;
    }

    public static Pose GetRightMiddleTip()
    {
        Instance.handsSubsystem.TryGetJoint(TrackedHandJoint.MiddleTip, XRNode.RightHand, out HandJointPose jointPose);
        return jointPose.Pose;
    }

    public static bool IsLeftIndexFingerPinching()
    {
        Vector3 thumbTipPosition = GetLeftThumbTip().position;
        Vector3 inderFingerTipPosition = GetLeftIndexTip().position;

        return CalculatePinching(thumbTipPosition, inderFingerTipPosition);
    }

    public static bool IsRightIndexFingerPinching()
    {
        Vector3 thumbTipPosition = GetRightThumbTip().position;
        Vector3 inderFingerTipPosition = GetRightIndexTip().position;

        return CalculatePinching(thumbTipPosition, inderFingerTipPosition);
    }

    public static bool IsLeftMiddleFingerPinching()
    {
        Vector3 thumbTipPosition = GetLeftThumbTip().position;
        Vector3 middleFingerTipPosition = GetLeftMiddleTip().position;

        return CalculatePinching(thumbTipPosition, middleFingerTipPosition);
    }

    public static bool IsRightMiddleFingerPinching()
    {
        Vector3 thumbTipPosition = GetRightThumbTip().position;
        Vector3 middleFingerTipPosition = GetRightMiddleTip().position;

        return CalculatePinching(thumbTipPosition, middleFingerTipPosition);
    }

    public static bool IsBothHandsIndexPinching()
    {
        return IsLeftIndexFingerPinching() && IsRightIndexFingerPinching();
    }

    static bool CalculatePinching(Vector3 thumbTipPosition, Vector3 fingerTipPosition)
    {
        float distance = Vector3.Distance(thumbTipPosition, fingerTipPosition);

        float pinchThreshold = 0.02f; // Adjust the threshold as needed
        bool isThumbPinchedToMiddle = distance < pinchThreshold;

        return isThumbPinchedToMiddle;
    }


    IEnumerator Start()
    {
        handsSubsystem = XRSubsystemHelpers.GetFirstRunningSubsystem<HandsAggregatorSubsystem>();

        yield return new WaitUntil(() => handsSubsystem != null);

    }
}