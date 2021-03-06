using System.Threading.Tasks;
using Core.Data;

namespace Core.Repositories.Impl;

public class UnitOfWork : IUnitOfWork
{
    private readonly DataContext _context;
    public IUserRepository Users { get; }
    public IStudyRepository Studies { get; }
    public ISeriesRepository Series { get; }
    public IInstanceRepository Instances { get; set; }
    public IMeasurementRepository Measurements { get; set; }

    public UnitOfWork(DataContext context)
    {
        _context = context;
        Users = new UserRepository(context);
        Studies = new StudyRepository(context);
        Series = new SeriesRepository(context);
        Instances = new InstanceRepository(context);
        Measurements = new MeasurementRepository(context);
    }

    public async Task CompleteAsync()
    {
        await _context.SaveChangesAsync();
    }
}