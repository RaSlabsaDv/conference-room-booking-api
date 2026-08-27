public interface IPricingCalculator
{
    Money Calculate
    (
        Room room, 
        DateTime startTime, 
        DateTime endTime, 
        IReadOnlyCollection<Service> selectedServices);
}