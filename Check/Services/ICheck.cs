using DAL.Models;

namespace Check.Services;

public interface ICheck
{
    Task<CheckLog?> Check(CheckList item);
    Task<CheckLog[]?> Check(CheckList[] item);
    string GetStatusAsync(object result);
}