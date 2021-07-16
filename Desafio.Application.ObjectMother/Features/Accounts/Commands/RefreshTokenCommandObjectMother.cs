using NddCargo.Integration.Application.Features.Accounts.Commands;

namespace NddCargo.Integration.Application.ObjectMother.Features.Accounts.Commands
{
    public class RefreshTokenCommandObjectMother
    {
        public static RefreshTokenCommand RefreshTokenCommand => new RefreshTokenCommand
        {
           RefreshToken = "qPH5Nn8XURXY7oIBEIKSE7q2qdvnDqig8dAj9OJ7xvk=",
           AccessToken = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoicGVyc29uYWwiLCJleHAiOjE2MDk5MzU3NDYsImlzcyI6Imh0dHBzOi8vbXl3ZWJhcGkuY29tIiwiYXVkIjoiaHR0cHM6Ly9teXdlYmFwaS5jb20ifQ.pznxAwPORbGDDkOCdeo2c4YxYI-H2ohIr7fONZ5nTgk"
        };
    }
}
