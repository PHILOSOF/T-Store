using Moq;
using T_Strore.Business.Services;
using T_Strore.Data;
using T_Strore.Data.Repository.Interfaces;
using T_Strore.Business.Services.Interfaces;

namespace T_Store.Business.Tests
{
    public class TransactionServicesTests
    {
        private TransactionServices _sut;
        private Mock<ITransactionRepository> _transactionRepositoryMock;
        private Mock<ICalculationService> _calculationService;


        [SetUp]
        public void Setup()
        {
            _transactionRepositoryMock = new Mock<ITransactionRepository>();
            _calculationService = new Mock<ICalculationService>();
            _sut = new TransactionServices(_transactionRepositoryMock.Object, _calculationService.Object);
        }

        [Test]
        public async Task AddDeposit_ValidRequestPassed_AddTransactionAndIdReturned()
        {
            //given
            _transactionRepositoryMock.Setup(t => t.AddTransaction(It.IsAny<TransactionDto>()))
            .ReturnsAsync(1);
            var expectedId = 1;

            var transaction = new TransactionDto()
            {
  
                AccountId = 1,
                Amount = 10,
                Currency =Currency.USD

            };   
    
           //when
            var actual = await _sut.AddDeposit(transaction);

            //then
           
            Assert.AreEqual(actual, expectedId);
            _transactionRepositoryMock.Verify(t => t.AddTransaction(transaction), Times.Once);
        }


        [Test]
        public async Task WithdrawDeposit_ValidRequestPassed_WithdrawAndIdReturned()
        {
            //given
            _transactionRepositoryMock.Setup(t => t.GetBalanceByAccountId(It.IsAny<int>()))
             .ReturnsAsync(100);
            _transactionRepositoryMock.Setup(t => t.AddTransaction(It.IsAny<TransactionDto>()))
            .ReturnsAsync(2);


            var expectedId = 2;

            var transaction = new TransactionDto()
            {

                AccountId = 1,
                Amount = 10,
                Currency = Currency.EUR

            };

            //when
            var actual = await _sut.WithdrawDeposit(transaction);

            //then

            Assert.AreEqual(actual, expectedId);
            _transactionRepositoryMock.Verify(t => t.AddTransaction(transaction), Times.Once);
        }
    }
}