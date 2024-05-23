using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Core.Models;

namespace Test.DAL.Repository
{
    public interface IEntityRepository
    {
        Task AddCandidateAsync(Candidate candidate);

        Task DeleteCandidateAsync(Candidate candidate);

        Task<Candidate?> GetCandidateAsync(string emailAddress);

        Task UpdateCandidateAsync(Candidate candidate);
    }
}
