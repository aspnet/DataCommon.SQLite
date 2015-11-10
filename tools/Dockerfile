FROM natemcmaster/aspnet-test:latest
RUN apt-get update -qq \
    && apt-get install -y libsqlite3-dev \
    && rm -rf /var/lib/apt/lists/*
ENV DNX_UNSTABLE_FEED https://www.myget.org/F/aspnetcidev/api/v2/
RUN bash -c "source $DNX_USER_HOME/dnvm/dnvm.sh \
    && dnvm install latest -u -alias default \
    && dnvm install latest -u -r coreclr"
ENV TEST_DIR /data
RUN mkdir -p $TEST_DIR
WORKDIR $TEST_DIR
COPY ./ $TEST_DIR
