using Electro.Corporation.Platform.Analytics.Domain.Model.Entities;
using Electro.Corporation.Platform.Analytics.Domain.Repositories;
using Electro.Corporation.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Electro.Corporation.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace Electro.Corporation.Platform.Analytics.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class ConsumptionRepository(AppDbContext context)
  : BaseRepository<Consumption>(context), IConsumptionRepository;
