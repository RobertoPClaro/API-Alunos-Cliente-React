using AlunosApi.Context;
using AlunosApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AlunosApi.Services
{
    public class AlunosService : IAlunosService
    {
        private readonly AppDbContext _context;

        public AlunosService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Aluno>> GetAlunos()
        {
            try
            {
                return await _context.Alunos.ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<Aluno>> GetAlunosByNome(string nome)
        {
            List<Aluno> alunos;
            if (!string.IsNullOrWhiteSpace(nome))
            {
                alunos = await _context.Alunos.Where(n => n.Nome.Contains(nome)).ToListAsync();
            }
            else
            {
                alunos = await GetAlunos();
            }
            return alunos;
        }
        public async Task<Aluno> GetAluno(int id)
        {
            Aluno aluno = await _context.Alunos.FindAsync(id);
            return aluno;
        }
        public async Task CreateAluno(Aluno aluno)
        {
            _context.Alunos.Add(aluno);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAluno(Aluno aluno)
        {
            _context.Entry(aluno).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAluno(Aluno aluno)
        {
            _context.Alunos.Remove(aluno);
            await _context.SaveChangesAsync();
        }
    }
}
