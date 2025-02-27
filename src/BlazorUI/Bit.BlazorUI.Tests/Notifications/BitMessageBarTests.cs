﻿using System.Collections.Generic;
using Bunit;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bit.BlazorUI.Tests.Notifications;

[TestClass]
public class BitMessageBarTests : BunitTestContext
{
    [DataTestMethod,
        DataRow(BitMessageBarType.Info),
        DataRow(BitMessageBarType.Blocked),
        DataRow(BitMessageBarType.Error),
        DataRow(BitMessageBarType.SevereWarning),
        DataRow(BitMessageBarType.Success),
        DataRow(BitMessageBarType.Warning)
    ]
    public void BitMessageBarShouldTakeCorrectType(BitMessageBarType messageBarType)
    {
        var component = RenderComponent<BitMessageBar>(parameters =>
        {
            parameters.Add(p => p.MessageBarType, messageBarType);
        });

        var bitMessageBar = component.Find(".bit-msb");

        var messageBarTypeClass = messageBarType switch
        {
            BitMessageBarType.Info => "bit-msb-info",
            BitMessageBarType.Success => "bit-msb-success",
            BitMessageBarType.Warning => "bit-msb-warning",
            BitMessageBarType.SevereWarning => "bit-msb-severe-warning",
            BitMessageBarType.Error => "bit-msb-error",
            BitMessageBarType.Blocked => "bit-msb-blocked",
            _ => "bit-msb-info"
        };

        Assert.IsTrue(bitMessageBar.ClassList.Contains(messageBarTypeClass));

        var icon = component.Find(".bit-msb-ict > i");

        Dictionary<BitMessageBarType, string> IconMap = new()
        {
            [BitMessageBarType.Info] = "Info",
            [BitMessageBarType.Warning] = "Info",
            [BitMessageBarType.Error] = "ErrorBadge",
            [BitMessageBarType.Blocked] = "Blocked2",
            [BitMessageBarType.SevereWarning] = "Warning",
            [BitMessageBarType.Success] = "Completed"
        };

        Assert.IsTrue(icon.ClassList.Contains($"bit-icon--{IconMap[messageBarType]}"));
    }

    [DataTestMethod,
        DataRow("Emoji2"),
        DataRow("WordLogo")
    ]
    public void BitMessageBarShouldRespectCustomIcon(string iconName)
    {
        var component = RenderComponent<BitMessageBar>(parameters =>
        {
            parameters.Add(p => p.MessageBarIconName, iconName);
        });

        var icon = component.Find(".bit-msb-ict > i");
        Assert.IsTrue(icon.ClassList.Contains($"bit-icon--{iconName}"));
    }

    [DataTestMethod,
        DataRow(true, true),
        DataRow(true, false),
        DataRow(false, true),
        DataRow(false, false)
    ]
    public void BitMessageBarShouldRespectMultiline(bool isMultiline, bool truncated)
    {
        var component = RenderComponent<BitMessageBar>(parameters =>
        {
            parameters.Add(p => p.IsMultiline, isMultiline);
            parameters.Add(p => p.Truncated, truncated);
        });

        var bitMessageBar = component.Find(".bit-msb > div");

        var messageBarMultilineType = isMultiline ? "bit-msb-mul" : "bit-msb-sin";
        Assert.IsTrue(bitMessageBar.ClassList.Contains(messageBarMultilineType));

        if (isMultiline is false && truncated)
        {
            var truncateButton = component.Find(".bit-msb-trn > button");
            var icon = component.Find(".bit-msb-trn button span i");

            Assert.IsTrue(icon.ClassList.Contains("bit-icon--DoubleChevronDown"));

            truncateButton.Click();

            Assert.IsTrue(icon.ClassList.Contains("bit-icon--DoubleChevronUp"));

            truncateButton.Click();

            Assert.IsTrue(icon.ClassList.Contains("bit-icon--DoubleChevronDown"));
        }
    }

    [DataTestMethod]
    public void BitMessageBarDismissButtonShouldWorkCorrect()
    {
        int currentCount = 0;
        var component = RenderComponent<BitMessageBar>(parameters =>
        {
            parameters.Add(p => p.OnDismiss, () => currentCount++);
        });

        var dismissButton = component.Find(".bit-msb-dim > button");

        dismissButton.Click();

        Assert.AreEqual(1, currentCount);
    }

    [DataTestMethod,
        DataRow("Emoji2"),
        DataRow("WordLogo")
    ]
    public void BitMessageBarShouldRespectCustomDismissIcon(string iconName)
    {
        var component = RenderComponent<BitMessageBar>(parameters =>
        {
            parameters.Add(p => p.DismissIconName, iconName);
            parameters.Add(p => p.OnDismiss, () => { });
        });

        var icon = component.Find(".bit-msb-dim button span i");

        Assert.IsTrue(icon.ClassList.Contains($"bit-icon--{iconName}"));
    }

    [DataTestMethod,
        DataRow("test dismiss aria label", false),
        DataRow("test dismiss aria label", true)
    ]
    public void BitMessageBarDismissButtonAriaLabelTest(string ariaLabel, bool isMultiline)
    {
        var component = RenderComponent<BitMessageBar>(parameters =>
        {
            parameters.Add(p => p.IsMultiline, isMultiline);
            parameters.Add(p => p.DismissButtonAriaLabel, ariaLabel);
            parameters.Add(p => p.OnDismiss, () => { });
        });

        var dismissButton = component.Find(".bit-msb-dim button");

        Assert.IsTrue(dismissButton.HasAttribute("aria-label"));
        Assert.AreEqual(ariaLabel, dismissButton.GetAttribute("aria-label"));

        Assert.IsTrue(dismissButton.HasAttribute("title"));
        Assert.AreEqual(ariaLabel, dismissButton.GetAttribute("title"));
    }

    [DataTestMethod,
        DataRow("test overflow aria label")
    ]
    public void BitMessageBarOverflowButtonAriaLabelTest(string ariaLabel)
    {
        var component = RenderComponent<BitMessageBar>(parameters =>
        {
            parameters.Add(p => p.IsMultiline, false);
            parameters.Add(p => p.Truncated, true);
            parameters.Add(p => p.OverflowButtonAriaLabel, ariaLabel);
        });

        var dismissButton = component.Find(".bit-msb-trn button");

        Assert.IsTrue(dismissButton.HasAttribute("aria-label"));
        Assert.AreEqual(ariaLabel, dismissButton.GetAttribute("aria-label"));
    }

    [DataTestMethod,
        DataRow("<div><button>Action</button></div>")
    ]
    public void BitMessageBarShouldRespectAction(string actions)
    {
        var component = RenderComponent<BitMessageBar>(parameter =>
        {
            parameter.Add(p => p.Actions, actions);
        });

        var actionsTemplate = component.Find(".bit-msb-act").ChildNodes;
        actionsTemplate.MarkupMatches(actions);
    }

    [DataTestMethod,
        DataRow("alert", BitMessageBarType.Info),
        DataRow("alert", BitMessageBarType.Error),
        DataRow("alert", BitMessageBarType.SevereWarning),
        DataRow("alert", BitMessageBarType.Success),
        DataRow("alert", BitMessageBarType.Warning),
        DataRow("alert", BitMessageBarType.Blocked),

        DataRow(null, BitMessageBarType.Info),
        DataRow(null, BitMessageBarType.Blocked),
        DataRow(null, BitMessageBarType.Error),
        DataRow(null, BitMessageBarType.SevereWarning),
        DataRow(null, BitMessageBarType.Success),
        DataRow(null, BitMessageBarType.Warning),
    ]
    public void BitMessageBarRoleTest(string role, BitMessageBarType type)
    {
        var component = RenderComponent<BitMessageBar>(parameter =>
        {
            parameter.Add(p => p.Role, role);
            parameter.Add(p => p.MessageBarType, type);
        });

        var textEl = component.Find(".bit-msb-txt");
        var expectedRole = role is not null ? role : BitMessageBarTests.GetRole(type);

        Assert.AreEqual(expectedRole, textEl.GetAttribute("role"));
    }

    private static string GetRole(BitMessageBarType type)
     => type switch
     {
         BitMessageBarType.Blocked or BitMessageBarType.Error or BitMessageBarType.SevereWarning => "alert",
         _ => "status",
     };
}
