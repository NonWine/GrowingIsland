using System.Collections.Generic;

public interface IDetectionHandler<TTarget>
{
    void Handle(List<TTarget> detectedObjects);
}
