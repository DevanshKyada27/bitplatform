﻿namespace Bit.Websites.Careers.Shared.Infra;

/// <summary>
/// https://bitplatform.dev/templates/hosting-models
/// </summary>
public class WebAppDeploymentTypeDetector
{
    public static WebAppDeploymentTypeDetector Current { get; set; } = new WebAppDeploymentTypeDetector();

    public virtual bool IsDefault()
    {
        return Mode == WebAppDeploymentType.Default;
    }

    public virtual bool IsStatic()
    {
        return Mode == WebAppDeploymentType.Static;
    }

    public virtual bool IsSsr()
    {
        return Mode == WebAppDeploymentType.Ssr;
    }

    public virtual WebAppDeploymentType Mode
    {
        get
        {
#if SSR
            return WebAppDeploymentType.Ssr;
#elif Static
            return WebAppDeploymentType.Static;
#else
            return WebAppDeploymentType.Default;
#endif
        }
    }
}
