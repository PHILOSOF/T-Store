using Moq;
using T_Strore.Business.Services;
using T_Strore.Data;
using T_Strore.Data.Repository.Interfaces;
using T_Strore.Business.Exceptions;


namespace T_Store.Business.Tests
{
    public class TransactionServicesTests
    {
        private TransactionServices _sut;
        private Mock<ITransactionRepository> _transactionRepositoryMock;

        [SetUp]
        public void Setup()
        {
            _transactionRepositoryMock = new Mock<ITransactionRepository>();
            _sut = new TransactionServices(_transactionRepositoryMock.Object);
        }

        [Test]
        public void AddDeposit_ValidRequestPassed_AddTransactionAndIdReturned()
        {
            //given
            _transactionRepositoryMock.Setup(t => t.AddTransaction(It.IsAny<TransactionDto>()))
            .Returns(1);
            var expectedId = 1;

            var transaction = new TransactionDto()
            {
  
                AccountId = 1,
                Amount = 10,
                Currency =Currency.USD

            };   
    
           //when
            var actual = _sut.AddDeposit(transaction);

            //then
           
            Assert.AreEqual(actual, expectedId);
            _transactionRepositoryMock.Verify(t => t.AddTransaction(transaction), Times.Once);
        }


        [Test]
        public void AddDeposit_TypeCurrencyDoesntMatch_ThrowBadRequestException()
        {
            //given
            _transactionRepositoryMock.Setup(t => t.GetCurrencyByAccountId(It.IsAny<int>()))
             .Returns(1);

         

            var transaction = new TransactionDto()
            {
                AccountId = 1,
                Amount = 10,
                Currency = Currency.JPY

            };

            //when, then 
            Assert.Throws<BadRequestException>(() => _sut.AddDeposit(transaction));
            _transactionRepositoryMock.Verify(t => t.AddTransaction(It.IsAny<TransactionDto>()), Times.Never);
        }

        [Test]
        public void WithdrawDeposit_ValidRequestPassed_WithdrawAndIdReturned()
        {
            //given
            _transactionRepositoryMock.Setup(t => t.GetCurrencyByAccountId(It.IsAny<int>()))
              .Returns(1);
            _transactionRepositoryMock.Setup(t => t.GetBalanceByAccountId(It.IsAny<int>()))
             .Returns(100);
            _transactionRepositoryMock.Setup(t => t.AddTransaction(It.IsAny<TransactionDto>()))
            .Returns(2);


            var expectedId = 2;

            var transaction = new TransactionDto()
            {

                AccountId = 1,
                Amount = 10,
                Currency = Currency.EUR

            };

            //when
            var actual = _sut.WithdrawDeposit(transaction);

            //then

            Assert.AreEqual(actual, expectedId);
            _transactionRepositoryMock.Verify(t => t.AddTransaction(transaction), Times.Once);
        }
    }
}